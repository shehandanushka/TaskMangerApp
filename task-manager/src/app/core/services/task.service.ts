import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TaskItem } from '../models/task.model';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private apiUrl = 'https://localhost:44399/api/tasks';

  constructor(
    private http: HttpClient,
    private auth: AuthService
  ) {}

  getTasks(
    sortBy?: string, sortDir?: string,
    filter?: string, search?: string
  ): Observable<TaskItem[]> {
    let params = new HttpParams();
    if (sortBy) params = params.set('sortBy', sortBy);
    if (sortDir) params = params.set('sortDir', sortDir);
    if (filter) params = params.set('filter', filter);
    if (search) params = params.set('search', search);

    return this.http.get<TaskItem[]>(this.apiUrl, {
      headers: this.auth.getAuthHeaders(), params
    });
  }

  getTask(id: number): Observable<TaskItem> {
    return this.http.get<TaskItem>(`${this.apiUrl}/${id}`, {
      headers: this.auth.getAuthHeaders()
    });
  }

  createTask(task: {
    title: string; description?: string; priority: string;
  }): Observable<TaskItem> {
    return this.http.post<TaskItem>(this.apiUrl, task, {
      headers: this.auth.getAuthHeaders()
    });
  }

  updateTask(id: number, task: any): Observable<TaskItem> {
    return this.http.put<TaskItem>(
      `${this.apiUrl}/${id}`, task,
      { headers: this.auth.getAuthHeaders() }
    );
  }

  toggleTask(id: number): Observable<TaskItem> {
    return this.http.patch<TaskItem>(
      `${this.apiUrl}/${id}/toggle`, {},
      { headers: this.auth.getAuthHeaders() }
    );
  }

  deleteTask(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      headers: this.auth.getAuthHeaders()
    });
  }
}