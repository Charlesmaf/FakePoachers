import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnimalDetailsAddComponent } from './animal-details-add.component';

describe('AnimalDetailsAddComponent', () => {
  let component: AnimalDetailsAddComponent;
  let fixture: ComponentFixture<AnimalDetailsAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnimalDetailsAddComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AnimalDetailsAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
