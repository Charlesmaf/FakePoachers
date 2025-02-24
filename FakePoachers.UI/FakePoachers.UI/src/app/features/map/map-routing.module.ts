import { Routes, RouterModule } from '@angular/router';
import { MapViewComponent } from './pages/map-view/map-view.component';

const routes: Routes = [
  { path: '', component: MapViewComponent, title: 'Map View' },
];

export default RouterModule.forChild(routes);
