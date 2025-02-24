import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AnimalService } from '../../../../core/services/http.services/animal.service';
import { GetAnimalsResponse } from '../../../../domains/responses/get.animals.response';
import { AnimalListDTO } from '../../../../domains/dtos/animallist.dto';
import { NgxPaginationModule } from 'ngx-pagination';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';

@Component({
  selector: 'app-animal-list',
  standalone: true,
  imports: [RouterLink, CommonModule, NgxPaginationModule, PaginationComponent],
  templateUrl: './animal-list.component.html',
  styleUrl: './animal-list.component.css',
})
export class AnimalListComponent {
  animals: AnimalListDTO[] = [];
  page: number = 1;
  pageSize: number = 10;
  loading: boolean = false;
  totalItems: number = 0;
  constructor(private animalService: AnimalService) {}
  ngOnInit(): void {
    this.loadAnimals();
  }

  loadAnimals(): void {
    this.loading = true;
    this.animalService.getAnimals(this.page, this.pageSize).subscribe({
      next: (data: GetAnimalsResponse) => {
        this.animals = data.content;
        this.totalItems = data.result;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading animals', err);
        this.loading = false;
      },
    });
  }

  onPageChange(page: number): void {
    this.page = page;
    this.loadAnimals();
  }
}
