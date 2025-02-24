import { Routes } from '@angular/router';
import { LayoutComponent } from './shared/components/layout/layout.component';

export const routes: Routes = [
  { path: '', redirectTo: '/animals', pathMatch: 'full' },
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: 'animals',
        loadComponent: () =>
          import(
            './features/animals/pages/animal-list/animal-list.component'
          ).then((m) => m.AnimalListComponent),
      },
      {
        path: 'animaldetails/:id',
        loadComponent: () =>
          import(
            './features/animals/pages/animal-detail/animal-detail.component'
          ).then((m) => m.AnimalDetailComponent),
      },
      {
        path: 'addanimal',
        loadComponent: () =>
          import(
            './features/animals/pages/animal-details-add/animal-details-add.component'
          ).then((m) => m.AnimalDetailsAddComponent),
      },
      {
        path: 'mapview',
        loadComponent: () =>
          import('./features/map/pages/map-view/map-view.component').then(
            (m) => m.MapViewComponent
          ),
      },
    ],
  },
];
