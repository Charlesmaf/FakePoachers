import {
  AfterViewInit,
  Component,
  inject,
  signal,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AnimalService } from '../../../../core/services/http.services/animal.service';
import { AnimalDetailsDTO } from '../../../../domains/dtos/animaldetails.dto';
import { GetAnimalDetailResponse } from '../../../../domains/responses/get.animaldetail.response';
import { CommonModule } from '@angular/common';
import { MapViewComponent } from '../../../map/pages/map-view/map-view.component';
import { GoogleMapsModule } from '@angular/google-maps';

@Component({
  selector: 'app-animal-detail',
  standalone: true,
  imports: [CommonModule, MapViewComponent, GoogleMapsModule],
  templateUrl: './animal-detail.component.html',
  styleUrl: './animal-detail.component.css',
})
export class AnimalDetailComponent implements AfterViewInit {
  @ViewChild(MapViewComponent) mapViewComponent!: MapViewComponent;
  private route = inject(ActivatedRoute);
  private animalService = inject(AnimalService);
  animalId = signal<number | null>(null);
  animaldetails: AnimalDetailsDTO | null | undefined;
  options: google.maps.MapOptions = {
    mapId: '43b87a66d68648fe',
    center: { lat: -26.2041, lng: 28.0456 },
    zoom: 4,
  };
  constructor() {
    this.route.paramMap.subscribe((params) => {
      const id = Number(params.get('id'));
      this.animalId.set(id);
      this.loadAnimal(id);
    });
  }
  ngAfterViewInit() {
    if (this.mapViewComponent) {
      console.log('MapViewComponent Instance:', this.mapViewComponent);
    }
  }
  private loadAnimal(id: number) {
    this.animalService.getAnimalById(id).subscribe({
      next: (data: GetAnimalDetailResponse) => {
        this.animaldetails = data.content;
        console.log('Animail Info :', data.content);
      },
      error: (err) => {
        console.error('Error loading animals', err);
      },
    });
  }
}
