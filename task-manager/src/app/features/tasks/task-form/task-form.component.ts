import { Component, inject, input, output, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TaskItem, TaskCreateDto, TaskUpdateDto } from '../../../core/models/task.model';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatCheckboxModule,
  ],
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.scss'],
})
export class TaskFormComponent {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<TaskFormComponent>, { optional: true });
  private dialogData = inject(MAT_DIALOG_DATA, { optional: true }) as { task?: TaskItem } | null;

  editingTask = input<TaskItem | null>(null);
  loading = input<boolean>(false);
  task: TaskItem | null = null;
  currentTask: TaskItem | null = null;
  submitting = signal(false);

  saved = output<TaskCreateDto | TaskUpdateDto>();
  cancelled = output<void>();

  form = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: [''],
    priority: ['Medium' as 'Low' | 'Medium' | 'High', Validators.required],
    isCompleted: [false],
  });

  constructor() {
    if (this.dialogData?.task) {
      this.currentTask = this.dialogData.task;
    }

    effect(() => {
      const task = this.currentTask ?? this.dialogData?.task ?? this.editingTask();
      this.submitting.set(false);

      if (task) {
        this.form.patchValue({
          title: task.title,
          description: task.description ?? '',
          priority: task.priority,
          isCompleted: task.isCompleted,
        });
      } else {
        this.form.reset({ priority: 'Medium', isCompleted: false });
      }
    });
  }

  get activeTask(): TaskItem | null {
    return this.currentTask ?? this.task ?? this.editingTask();
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);
    const { title, description, priority, isCompleted } = this.form.value;
    const task = this.activeTask;

    if (task) {
      const dto: TaskUpdateDto = {
        title: title!,
        description: description ?? undefined,
        priority: priority!,
        isCompleted: isCompleted!,
      };
      this.saved.emit(dto);
    } else {
      const dto: TaskCreateDto = {
        title: title!,
        description: description ?? undefined,
        priority: priority!,
      };
      this.saved.emit(dto);
    }
  }

  onCancel(): void {
    this.form.reset({ priority: 'Medium', isCompleted: false });
    this.cancelled.emit();
    if (this.dialogRef) {
      this.dialogRef.close();
    }
  }
}