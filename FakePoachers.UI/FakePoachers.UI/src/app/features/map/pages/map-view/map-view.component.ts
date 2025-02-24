import { AfterViewInit, Component, ViewChild } from '@angular/core';
import * as L from 'leaflet';
import { GoogleMapsModule } from '@angular/google-maps';
import { LocationService } from '../../../../core/services/http.services/location.service';
import { GetLocationsResponse } from '../../../../domains/responses/get.location.response';
import { LocationDTO } from '../../../../domains/dtos/locations.dto';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../../../core/services/toast.service';
import { StatusCodes } from '../../../../domains/enums/statuscode';
@Component({
  selector: 'app-map-view',
  standalone: true,
  imports: [GoogleMapsModule, CommonModule],
  templateUrl: './map-view.component.html',
  styleUrl: './map-view.component.css',
})
export class MapViewComponent implements AfterViewInit {
  @ViewChild('map', { static: false }) map!: google.maps.Map;
  options: google.maps.MapOptions = {
    mapId: '43b87a66d68648fe',
    center: { lat: -26.2041, lng: 28.0456 },
    zoom: 5,
  };
  locations: LocationDTO[] = [];
  ngOnInit(): void {
    this.loadLocations();
  }
  constructor(private locationService: LocationService) {}
  ngAfterViewInit(): void {
    if (this.map) {
      console.log('Google Map Component:', this.map);
    }
  }
  private loadLocations() {
    this.locationService.getLocations().subscribe({
      next: (response: GetLocationsResponse) => {
        if (response.responseCode == StatusCodes.OK) {
          this.locations = response.content;
        }
      },
      error: (err) => {
        console.error('Error loading animals', err);
      },
    });
  }
}
