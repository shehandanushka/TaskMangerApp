export interface TaskItem {
  id: number;
  title: string;
  description?: string;
  isCompleted: boolean;
  priority: 'Low' | 'Medium' | 'High';
  createdAt: string;
  updatedAt?: string;
}

export interface TaskCreateDto {
  title: string;
  description?: string;
  priority: 'Low' | 'Medium' | 'High';
}

export interface TaskUpdateDto extends TaskCreateDto {
  isCompleted: boolean;
}