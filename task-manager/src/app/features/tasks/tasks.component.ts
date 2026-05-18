import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { take } from 'rxjs';

import { TaskListComponent } from './task-list/task-list.component';
import { TaskFormComponent } from './task-form/task-form.component';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog.component';
import { TaskService } from '../../core/services/task.service';
import { AuthService } from '../../core/services/auth.service';
import { TaskItem, TaskCreateDto, TaskUpdateDto } from '../../core/models/task.model';

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatDialogModule,
    MatProgressBarModule,
    MatTooltipModule,
    TaskListComponent,
  ],
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss'],
})
export class TasksComponent implements OnInit {
  private taskService = inject(TaskService);
  private auth = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);
  private dialog = inject(MatDialog);

  tasks = signal<TaskItem[]>([]);
  editingTask = signal<TaskItem | null>(null);
  loading = signal(false);
  formLoading = signal(false);

  get username(): string { return this.auth.getUsername(); }

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.loading.set(true);
    this.taskService.getTasks().subscribe({
      next: (tasks) => { this.tasks.set(tasks); this.loading.set(false); },
      error: () => {
        this.loading.set(false);
        this.snackBar.open('Failed to load tasks', '✕', {
          duration: 3000,
          verticalPosition: 'top',
          horizontalPosition: 'center',
          panelClass: ['app-snackbar'],
        });
      },
    });
  }

  onEditTask(task: TaskItem): void {
    this.openTaskDialog(task);
  }

  openTaskDialog(task?: TaskItem): void {
    this.editingTask.set(task ?? null);

    const dialogRef = this.dialog.open(TaskFormComponent, {
      width: '520px',
      maxWidth: '95vw',
      autoFocus: false,
      data: { task },
    });

    const component = dialogRef.componentInstance;
    component.task = task ?? null;
    if (task) {
      component.form.patchValue({
        title: task.title,
        description: task.description ?? '',
        priority: task.priority,
        isCompleted: task.isCompleted,
      });
    }

    const savedSubscription = component.saved.subscribe((dto: TaskCreateDto | TaskUpdateDto) => {
      savedSubscription.unsubscribe();
      this.onSaved(dto, dialogRef);
    });

    const cancelledSubscription = component.cancelled.subscribe(() => {
      cancelledSubscription.unsubscribe();
      dialogRef.close();
    });

    dialogRef.afterClosed().pipe(take(1)).subscribe(() => this.editingTask.set(null));
  }

  onToggleTask(task: TaskItem): void {
    this.taskService.toggleTask(task.id).subscribe({
      next: (updated) => {
        this.tasks.update(list =>
          list.map(t => t.id === updated.id ? updated : t)
        );
        const msg = updated.isCompleted ? 'Task completed!' : 'Task marked as pending';
        this.snackBar.open(msg, '✕', {
          duration: 2000,
          verticalPosition: 'top',
          horizontalPosition: 'center',
          panelClass: ['app-snackbar'],
        });
      },
      error: () => this.snackBar.open('Update failed', '✕', {
        duration: 3000,
        verticalPosition: 'top',
        horizontalPosition: 'center',
        panelClass: ['app-snackbar', 'snack-error'],
      }),
    });
  }

  onDeleteTask(task: TaskItem): void {
    const ref = this.dialog.open(ConfirmDialogComponent, {
      width: '360px',
      data: { title: 'Delete Task', message: `Are you sure you want to delete the task "${task.title}"?` },
    });

    ref.afterClosed().subscribe((confirmed) => {
      if (!confirmed) return;
      this.taskService.deleteTask(task.id).subscribe({
        next: () => {
          this.tasks.update(list => list.filter(t => t.id !== task.id));
          if (this.editingTask()?.id === task.id) this.editingTask.set(null);
          this.snackBar.open('Task deleted', '✕', {
            duration: 2000,
            verticalPosition: 'top',
            horizontalPosition: 'center',
            panelClass: ['app-snackbar'],
          });
        },
        error: () => this.snackBar.open('Delete failed', '✕', {
          duration: 3000,
          verticalPosition: 'top',
          horizontalPosition: 'center',
          panelClass: ['app-snackbar', 'snack-error'],
        }),
      });
    });
  }

  onSaved(dto: TaskCreateDto | TaskUpdateDto, dialogRef?: MatDialogRef<TaskFormComponent>): void {
    const editing = this.editingTask();
    this.formLoading.set(true);

    if (editing) {
      this.taskService.updateTask(editing.id, dto as TaskUpdateDto).subscribe({
        next: (updated) => {
          this.tasks.update(list => list.map(t => t.id === updated.id ? updated : t));
          this.editingTask.set(null);
          this.formLoading.set(false);
          this.snackBar.open('Task updated!', '✕', {
            duration: 2000,
            verticalPosition: 'top',
            horizontalPosition: 'center',
            panelClass: ['app-snackbar'],
          });
          if (dialogRef) {
            dialogRef.close();
          }
        },
        error: () => {
          this.formLoading.set(false);
          this.snackBar.open('Update failed', '✕', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center',
            panelClass: ['app-snackbar', 'snack-error'],
          });
        },
      });
    } else {
      this.taskService.createTask(dto as TaskCreateDto).subscribe({
        next: (created) => {
          this.tasks.update(list => [created, ...list]);
          this.formLoading.set(false);
          this.snackBar.open('Task added!', '✕', {
            duration: 2000,
            verticalPosition: 'top',
            horizontalPosition: 'center',
            panelClass: ['app-snackbar'],
          });
          if (dialogRef) {
            dialogRef.close();
          }
        },
        error: () => {
          this.formLoading.set(false);
          this.snackBar.open('Create failed', '✕', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center',
            panelClass: ['app-snackbar', 'snack-error'],
          });
        },
      });
    }
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}