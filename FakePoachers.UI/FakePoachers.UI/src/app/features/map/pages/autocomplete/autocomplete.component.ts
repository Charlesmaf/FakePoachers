import {
  Component,
  ElementRef,
  EventEmitter,
  NgZone,
  Output,
  ViewChild,
} from '@angular/core';

@Component({
  selector: 'app-autocomplete',
  standalone: true,
  imports: [],
  templateUrl: './autocomplete.component.html',
  styleUrl: './autocomplete.component.css',
})
export class AutocompleteComponent {
  @ViewChild('addressInput') addressInput!: ElementRef;
  @Output() locationSelected = new EventEmitter<{
    latitude: number;
    longitude: number;
    address: string;
  }>();

  autocomplete: any;
  constructor(private ngZone: NgZone) {}
  ngOnInit() {
    this.loadGoogleMapsAPI();
  }
  loadGoogleMapsAPI() {
    const script = document.createElement('script');
    script.src = `https://maps.googleapis.com/maps/api/js?key=AIzaSyCyPRbrLFQ_dUqdhFPGgpm3sOuUzft5uC0&libraries=places`;
    script.async = true;
    script.defer = true;
    document.body.appendChild(script);

    script.onload = () => {
      this.initAutocomplete();
    };
  }
  initAutocomplete() {
    this.autocomplete = new google.maps.places.Autocomplete(
      this.addressInput.nativeElement,
      {
        componentRestrictions: { country: 'za' },
        fields: ['address_components', 'geometry', 'name'],
        types: ['address'],
      }
    );

    this.autocomplete.addListener('place_changed', () => {
      const place = this.autocomplete.getPlace();

      this.ngZone.run(() => {
        if (place.geometry) {
          const latitude = place.geometry.location.lat();
          const longitude = place.geometry.location.lng();
          const addressComponents = place.address_components;
          const address = addressComponents
            .map((component: any) => component.long_name)
            .join(' ');

          let street = '';
          let city = '';
          for (const component of addressComponents) {
            if (component.types.includes('route')) {
              street = component.long_name;
            } else if (component.types.includes('locality')) {
              city = component.long_name;
            }
          }

          this.locationSelected.emit({ latitude, longitude, address });
        } else {
          console.log("User didn't select a place");
        }
      });
    });
  }
}
