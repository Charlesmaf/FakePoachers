import { Routes, RouterModule } from '@angular/router';
import { AnimalListComponent } from './pages/animal-list/animal-list.component';
import { AnimalDetailComponent } from './pages/animal-detail/animal-detail.component';
import { AnimalDetailsAddComponent } from './pages/animal-details-add/animal-details-add.component';

const routes: Routes = [
  { path: '', component: AnimalListComponent, title: 'Animals' },
  {
    path: 'details/:id',
    component: AnimalDetailComponent,
    title: 'Animal Details',
  },
  { path: 'add', component: AnimalDetailsAddComponent, title: 'Add Animal' },
];

export default RouterModule.forChild(routes);
