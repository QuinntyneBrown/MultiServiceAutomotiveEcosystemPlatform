import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiServiceAutomotiveEcosystemComponents } from './multi-service-automotive-ecosystem-components';

describe('MultiServiceAutomotiveEcosystemComponents', () => {
  let component: MultiServiceAutomotiveEcosystemComponents;
  let fixture: ComponentFixture<MultiServiceAutomotiveEcosystemComponents>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MultiServiceAutomotiveEcosystemComponents]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MultiServiceAutomotiveEcosystemComponents);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
