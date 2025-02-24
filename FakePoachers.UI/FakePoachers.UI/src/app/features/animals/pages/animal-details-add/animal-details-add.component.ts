import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MapViewComponent } from '../../../map/pages/map-view/map-view.component';
import { AutocompleteComponent } from '../../../map/pages/autocomplete/autocomplete.component';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AnimalService } from '../../../../core/services/http.services/animal.service';
import { PostAddAnimalResponse } from '../../../../domains/responses/post.addanimal.response';
import { StatusCodes } from '../../../../domains/enums/statuscode';
import { ToastService } from '../../../../core/services/toast.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-animal-details-add',
  standalone: true,
  imports: [
    CommonModule,
    MapViewComponent,
    AutocompleteComponent,
    ReactiveFormsModule,
  ],
  providers: [AnimalService],
  templateUrl: './animal-details-add.component.html',
  styleUrl: './animal-details-add.component.css',
})
export class AnimalDetailsAddComponent implements OnInit {
  animalForm!: FormGroup;
  selectedFile: File | null = null;
  imagePreview: string | ArrayBuffer | null = null;
  selectedLocation: { latitude: number; longitude: number } | null = null;
  animal = {
    name: '',
    latitude: 0,
    longitude: 0,
  };
  constructor(
    private fb: FormBuilder,
    private animalService: AnimalService,
    private toastService: ToastService,
    private router: Router
  ) {}
  ngOnInit(): void {
    this.animalForm = this.fb.group({
      name: ['', Validators.required],
      location: this.fb.group({
        latitude: [0, Validators.required],
        longitude: [0, Validators.required],
        address: ['', Validators.required],
      }),
      imageUrl: [''],
    });
  }
  onLocationSelected(location: { latitude: number; longitude: number }) {
    this.selectedLocation = location;
    this.animalForm.patchValue({ location: location });
  }
  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      if (!file.type.startsWith('image/')) {
        this.toastService.showToast(
          'Please select a valid image file.',
          'danger'
        );
        return;
      }

      const maxSize = 2 * 1024 * 1024; // 2MB
      if (file.size > maxSize) {
        this.toastService.showToast(
          'Image size should not exceed 2MB.',
          'danger'
        );
        return;
      }

      this.selectedFile = file;
      this.previewImage(file);
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result as string;
        this.animalForm.patchValue({ imageUrl: this.imagePreview });
      };
      reader.readAsDataURL(file);
    }
  }
  previewImage(file: File): void {
    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview = reader.result;
    };
    reader.readAsDataURL(file);
  }
  submitRequest() {
    if (this.animalForm.valid) {
      const formData = new FormData();
      formData.append('Name', this.animalForm.get('name')?.value);
      formData.append(
        'Latitude',
        this.animalForm.get('location')?.value.latitude
      );
      formData.append(
        'Longitude',
        this.animalForm.get('location')?.value.longitude
      );
      formData.append(
        'Address',
        this.animalForm.get('location')?.value.address
      );
      if (this.selectedFile) {
        formData.append('ImageFile', this.selectedFile);
      }

      this.animalService.addAnimal(formData).subscribe({
        next: (response: PostAddAnimalResponse) => {
          if (response.responseCode == StatusCodes.OK) {
            this.toastService.showToast(
              'Animal added successfully!',
              'success'
            );
            this.animalForm.reset();
            this.selectedFile = null;
            this.imagePreview = null;
            setTimeout(() => {
              this.router.navigate(['/animals']);
            }, 1000);
          } else {
            this.toastService.showToast('Failed to add animal!', 'danger');
          }
        },
        error: (err) => {
          console.error('Error adding animal', err);
        },
      });
    } else {
      this.toastService.showToast(
        'Please fill in all requird fields',
        'warning'
      );
    }
  }
}
