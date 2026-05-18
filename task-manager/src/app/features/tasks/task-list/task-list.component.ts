import {
  Component, inject, input, output,
  ViewChild, AfterViewInit, OnChanges,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { FormsModule } from '@angular/forms';
import { TaskItem } from '../../../core/models/task.model';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatTooltipModule,
    MatCardModule,
  ],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss'],
})
export class TaskListComponent implements AfterViewInit, OnChanges {
  @ViewChild(MatSort) sort!: MatSort;

  tasks = input<TaskItem[]>([]);
  selectedTaskId = input<number | null>(null);

  editTask = output<TaskItem>();
  deleteTask = output<TaskItem>();
  toggleTask = output<TaskItem>();

  displayedColumns = ['title', 'priority', 'isCompleted', 'createdAt', 'actions'];
  dataSource = new MatTableDataSource<TaskItem>([]);

  openTask = output<void>();

  searchText = '';
  statusFilter = 'all';
  priorityFilter = 'all';

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }

  ngOnChanges(): void {
    this.dataSource.data = this.tasks();
    this.applyFilter();
  }

  applyFilter(): void {
    this.dataSource.filterPredicate = (task: TaskItem) => {
      const matchesSearch =
        !this.searchText ||
        task.title.toLowerCase().includes(this.searchText.toLowerCase()) ||
        (task.description ?? '').toLowerCase().includes(this.searchText.toLowerCase());

      const matchesStatus =
        this.statusFilter === 'all' ||
        (this.statusFilter === 'completed' && task.isCompleted) ||
        (this.statusFilter === 'pending' && !task.isCompleted);

      const matchesPriority =
        this.priorityFilter === 'all' || task.priority === this.priorityFilter;

      return matchesSearch && matchesStatus && matchesPriority;
    };
    // Trigger filterPredicate re-evaluation
    this.dataSource.filter = `${this.searchText}|${this.statusFilter}|${this.priorityFilter}`;
  }

  onEdit(task: TaskItem): void   { this.editTask.emit(task); }
  onDelete(task: TaskItem): void { this.deleteTask.emit(task); }
  onToggle(task: TaskItem): void { this.toggleTask.emit(task); }
}