import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { SpecialtyManagement, Specialty, SpecialtyCatalogItem, Certification, SpecialtyCategory } from './specialty-management';

describe('SpecialtyManagement', () => {
  let component: SpecialtyManagement;
  let fixture: ComponentFixture<SpecialtyManagement>;

  const mockCatalogItems: SpecialtyCatalogItem[] = [
    {
      id: 'cat-1',
      name: 'Engine Repair',
      description: 'Complete engine diagnostics and repair',
      icon: '🔧',
      category: SpecialtyCategory.ENGINE,
      keywords: ['motor', 'engine', 'pistons'],
      isPopular: true
    },
    {
      id: 'cat-2',
      name: 'Brake Service',
      description: 'Brake pad replacement and rotor resurfacing',
      icon: '🛑',
      category: SpecialtyCategory.BRAKES,
      keywords: ['brakes', 'pads', 'rotors']
    },
    {
      id: 'cat-3',
      name: 'Electrical Systems',
      description: 'Wiring, batteries, and electrical diagnostics',
      icon: '⚡',
      category: SpecialtyCategory.ELECTRICAL,
      keywords: ['wiring', 'battery', 'alternator']
    }
  ];

  const mockSpecialties: Specialty[] = [
    {
      id: 'spec-1',
      name: 'Oil Change',
      icon: '🛢️',
      yearsOfExperience: 5,
      order: 0,
      isCustom: false,
      createdAt: new Date(),
      updatedAt: new Date()
    },
    {
      id: 'spec-2',
      name: 'Tire Rotation',
      icon: '🔄',
      yearsOfExperience: 3,
      order: 1,
      isCustom: false,
      createdAt: new Date(),
      updatedAt: new Date()
    }
  ];

  const mockCertifications: Certification[] = [
    {
      id: 'cert-1',
      name: 'ASE Certification',
      fileName: 'ase-cert.pdf',
      fileUrl: '/files/ase-cert.pdf',
      fileSize: 1024000,
      mimeType: 'application/pdf',
      uploadedAt: new Date(),
      isVerified: true
    }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SpecialtyManagement, ReactiveFormsModule, DragDropModule]
    }).compileComponents();

    fixture = TestBed.createComponent(SpecialtyManagement);
    component = fixture.componentInstance;
    component.availableSpecialties = [...mockCatalogItems];
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Component Rendering', () => {
    it('should render empty state when no specialties', () => {
      component.currentSpecialties = [];
      fixture.detectChanges();

      const emptyState = fixture.nativeElement.querySelector('.empty-state');
      expect(emptyState).toBeTruthy();
      expect(emptyState.textContent).toContain('No Specialties Added Yet');
    });

    it('should render specialty list when specialties exist', () => {
      component.currentSpecialties = [...mockSpecialties];
      fixture.detectChanges();

      const specialtyItems = fixture.nativeElement.querySelectorAll('.specialty-item');
      expect(specialtyItems.length).toBe(2);
    });

    it('should display specialty name and experience', () => {
      component.currentSpecialties = [...mockSpecialties];
      fixture.detectChanges();

      const firstItem = fixture.nativeElement.querySelector('.specialty-item');
      expect(firstItem.textContent).toContain('Oil Change');
      expect(firstItem.textContent).toContain('5 years experience');
    });

    it('should show loading state', () => {
      component.isLoading = true;
      fixture.detectChanges();

      const loadingState = fixture.nativeElement.querySelector('.loading-state');
      expect(loadingState).toBeTruthy();
    });

    it('should render certifications list', () => {
      component.certifications = [...mockCertifications];
      fixture.detectChanges();

      const certCards = fixture.nativeElement.querySelectorAll('.certification-card');
      expect(certCards.length).toBe(1);
    });

    it('should display limit indicators', () => {
      component.currentSpecialties = [...mockSpecialties];
      component.maxSpecialties = 10;
      fixture.detectChanges();

      const limitIndicator = fixture.nativeElement.querySelector('.limit-indicator');
      expect(limitIndicator.textContent).toContain('2 of 10');
    });
  });

  describe('Modal Behavior', () => {
    it('should open modal on Add Specialty click', () => {
      const addBtn = fixture.nativeElement.querySelector('.btn--primary');
      addBtn.click();
      fixture.detectChanges();

      expect(component.isModalOpen()).toBe(true);
      const modal = fixture.nativeElement.querySelector('.modal');
      expect(modal).toBeTruthy();
    });

    it('should close modal on close button click', () => {
      component.openModal();
      fixture.detectChanges();

      const closeBtn = fixture.nativeElement.querySelector('.modal-close');
      closeBtn.click();
      fixture.detectChanges();

      expect(component.isModalOpen()).toBe(false);
    });

    it('should close modal on overlay click', () => {
      component.openModal();
      fixture.detectChanges();

      const overlay = fixture.nativeElement.querySelector('.modal-overlay');
      overlay.click();
      fixture.detectChanges();

      expect(component.isModalOpen()).toBe(false);
    });

    it('should not open modal when at max specialties', () => {
      component.currentSpecialties = Array(10).fill(null).map((_, i) => ({
        ...mockSpecialties[0],
        id: `spec-${i}`,
        order: i
      }));
      component.maxSpecialties = 10;
      fixture.detectChanges();

      component.openModal();
      expect(component.isModalOpen()).toBe(false);
    });

    it('should reset selection on modal close', () => {
      component.openModal();
      component.toggleSpecialtySelection('cat-1');
      expect(component.selectedSpecialtyIds().size).toBe(1);

      component.closeModal();
      expect(component.selectedSpecialtyIds().size).toBe(0);
    });
  });

  describe('Search Functionality', () => {
    beforeEach(() => {
      component.openModal();
      fixture.detectChanges();
    });

    it('should filter catalog based on search query', () => {
      component.searchQuery.set('engine');
      fixture.detectChanges();

      expect(component.filteredSpecialties().length).toBe(1);
      expect(component.filteredSpecialties()[0].name).toBe('Engine Repair');
    });

    it('should search by keywords', () => {
      component.searchQuery.set('battery');
      fixture.detectChanges();

      expect(component.filteredSpecialties().length).toBe(1);
      expect(component.filteredSpecialties()[0].name).toBe('Electrical Systems');
    });

    it('should be case-insensitive', () => {
      component.searchQuery.set('BRAKE');
      fixture.detectChanges();

      expect(component.filteredSpecialties().length).toBe(1);
      expect(component.filteredSpecialties()[0].name).toBe('Brake Service');
    });

    it('should show no results message when no matches', () => {
      component.searchQuery.set('nonexistent');
      fixture.detectChanges();

      const noResults = fixture.nativeElement.querySelector('.no-results');
      expect(noResults).toBeTruthy();
    });

    it('should clear search on clear button click', () => {
      component.searchQuery.set('engine');
      fixture.detectChanges();

      component.clearSearch();
      expect(component.searchQuery()).toBe('');
      expect(component.filteredSpecialties().length).toBe(3);
    });
  });

  describe('Selection', () => {
    beforeEach(() => {
      component.openModal();
      fixture.detectChanges();
    });

    it('should toggle selection on catalog item click', () => {
      component.toggleSpecialtySelection('cat-1');
      expect(component.isSelected('cat-1')).toBe(true);

      component.toggleSpecialtySelection('cat-1');
      expect(component.isSelected('cat-1')).toBe(false);
    });

    it('should allow multiple selections', () => {
      component.toggleSpecialtySelection('cat-1');
      component.toggleSpecialtySelection('cat-2');
      fixture.detectChanges();

      expect(component.selectedSpecialtyIds().size).toBe(2);
    });

    it('should prevent selecting already added specialty', () => {
      component.currentSpecialties = [{
        ...mockSpecialties[0],
        id: 'cat-1'
      }];
      fixture.detectChanges();

      component.toggleSpecialtySelection('cat-1');
      expect(component.isSelected('cat-1')).toBe(false);
    });

    it('should enforce max specialties limit', () => {
      component.currentSpecialties = Array(9).fill(null).map((_, i) => ({
        ...mockSpecialties[0],
        id: `existing-${i}`,
        order: i
      }));
      component.maxSpecialties = 10;
      fixture.detectChanges();

      component.toggleSpecialtySelection('cat-1');
      expect(component.selectedSpecialtyIds().size).toBe(1);

      component.toggleSpecialtySelection('cat-2');
      // Should be rejected - would exceed max
      expect(component.selectedSpecialtyIds().size).toBe(1);
    });
  });

  describe('Custom Specialty Form', () => {
    beforeEach(() => {
      component.openModal();
      fixture.detectChanges();
    });

    it('should validate specialty name required', () => {
      const nameControl = component.customSpecialtyForm.get('name');
      nameControl?.setValue('');
      nameControl?.markAsTouched();

      expect(nameControl?.hasError('required')).toBe(true);
    });

    it('should validate name max length', () => {
      const nameControl = component.customSpecialtyForm.get('name');
      nameControl?.setValue('a'.repeat(101));
      nameControl?.markAsTouched();

      expect(nameControl?.hasError('maxlength')).toBe(true);
    });

    it('should validate years of experience range', () => {
      const yearsControl = component.customSpecialtyForm.get('yearsOfExperience');

      yearsControl?.setValue(-1);
      expect(yearsControl?.hasError('min')).toBe(true);

      yearsControl?.setValue(100);
      expect(yearsControl?.hasError('max')).toBe(true);

      yearsControl?.setValue(50);
      expect(yearsControl?.valid).toBe(true);
    });

    it('should reset form on modal close', () => {
      component.customSpecialtyForm.patchValue({
        name: 'Test Specialty',
        yearsOfExperience: 5
      });

      component.closeModal();
      component.openModal();

      expect(component.customSpecialtyForm.get('name')?.value).toBeFalsy();
    });
  });

  describe('Adding Specialties', () => {
    let addSpecialtiesSpy: jasmine.Spy;

    beforeEach(() => {
      addSpecialtiesSpy = spyOn(component.addSpecialties, 'emit');
      component.openModal();
      fixture.detectChanges();
    });

    it('should emit addSpecialties with selected catalog items', () => {
      component.toggleSpecialtySelection('cat-1');
      component.toggleSpecialtySelection('cat-2');
      component.addSelectedSpecialties();

      expect(addSpecialtiesSpy).toHaveBeenCalled();
      const emittedSpecialties = addSpecialtiesSpy.calls.first().args[0];
      expect(emittedSpecialties.length).toBe(2);
      expect(emittedSpecialties[0].name).toBe('Engine Repair');
    });

    it('should emit addSpecialties with custom specialty', () => {
      component.customSpecialtyForm.patchValue({
        name: 'Custom Service',
        yearsOfExperience: 7
      });
      component.addSelectedSpecialties();

      expect(addSpecialtiesSpy).toHaveBeenCalled();
      const emittedSpecialties = addSpecialtiesSpy.calls.first().args[0];
      expect(emittedSpecialties.length).toBe(1);
      expect(emittedSpecialties[0].name).toBe('Custom Service');
      expect(emittedSpecialties[0].yearsOfExperience).toBe(7);
      expect(emittedSpecialties[0].isCustom).toBe(true);
    });

    it('should prevent duplicate custom specialty names', () => {
      component.currentSpecialties = [{
        ...mockSpecialties[0],
        name: 'Existing Service'
      }];
      component.customSpecialtyForm.patchValue({
        name: 'existing service',
        yearsOfExperience: 2
      });
      component.addSelectedSpecialties();

      expect(addSpecialtiesSpy).not.toHaveBeenCalled();
    });

    it('should not emit when nothing selected', () => {
      component.addSelectedSpecialties();
      expect(addSpecialtiesSpy).not.toHaveBeenCalled();
    });

    it('should close modal after successful add', () => {
      component.toggleSpecialtySelection('cat-1');
      component.addSelectedSpecialties();

      expect(component.isModalOpen()).toBe(false);
    });
  });

  describe('Removing Specialties', () => {
    let removeSpecialtySpy: jasmine.Spy;

    beforeEach(() => {
      removeSpecialtySpy = spyOn(component.removeSpecialty, 'emit');
      component.currentSpecialties = [...mockSpecialties];
      fixture.detectChanges();
    });

    it('should show confirmation on remove click', () => {
      component.confirmRemove('spec-1');
      fixture.detectChanges();

      expect(component.confirmRemoveId()).toBe('spec-1');
      const confirmDialog = fixture.nativeElement.querySelector('.confirm-remove');
      expect(confirmDialog).toBeTruthy();
    });

    it('should emit removeSpecialty on confirm', () => {
      component.confirmRemove('spec-1');
      component.onRemoveSpecialty('spec-1');

      expect(removeSpecialtySpy).toHaveBeenCalledWith('spec-1');
    });

    it('should cancel removal on cancel click', () => {
      component.confirmRemove('spec-1');
      component.cancelRemove();

      expect(component.confirmRemoveId()).toBeNull();
      expect(removeSpecialtySpy).not.toHaveBeenCalled();
    });
  });

  describe('Drag and Drop', () => {
    let reorderSpy: jasmine.Spy;

    beforeEach(() => {
      reorderSpy = spyOn(component.reorderSpecialties, 'emit');
      component.currentSpecialties = [...mockSpecialties];
      fixture.detectChanges();
    });

    it('should emit reordered specialties on drop', () => {
      const event = {
        previousIndex: 0,
        currentIndex: 1
      } as any;

      component.onDrop(event);

      expect(reorderSpy).toHaveBeenCalled();
      const reorderedSpecialties = reorderSpy.calls.first().args[0];
      expect(reorderedSpecialties[0].id).toBe('spec-2');
      expect(reorderedSpecialties[1].id).toBe('spec-1');
    });

    it('should update order property after reorder', () => {
      const event = {
        previousIndex: 0,
        currentIndex: 1
      } as any;

      component.onDrop(event);

      const reorderedSpecialties = reorderSpy.calls.first().args[0];
      expect(reorderedSpecialties[0].order).toBe(0);
      expect(reorderedSpecialties[1].order).toBe(1);
    });

    it('should not emit when dropping in same position', () => {
      const event = {
        previousIndex: 1,
        currentIndex: 1
      } as any;

      component.onDrop(event);
      expect(reorderSpy).not.toHaveBeenCalled();
    });
  });

  describe('Certification Upload', () => {
    let addCertSpy: jasmine.Spy;

    beforeEach(() => {
      addCertSpy = spyOn(component.addCertification, 'emit');
      fixture.detectChanges();
    });

    it('should accept valid PDF file', fakeAsync(() => {
      const file = new File(['content'], 'test.pdf', { type: 'application/pdf' });
      (component as any).handleFileUpload(file);

      tick(1500);

      expect(addCertSpy).toHaveBeenCalledWith(file);
    }));

    it('should accept valid image files', fakeAsync(() => {
      const jpgFile = new File(['content'], 'cert.jpg', { type: 'image/jpeg' });
      (component as any).handleFileUpload(jpgFile);

      tick(1500);

      expect(addCertSpy).toHaveBeenCalledWith(jpgFile);
    }));

    it('should reject invalid file types', fakeAsync(() => {
      const file = new File(['content'], 'test.doc', { type: 'application/msword' });
      (component as any).handleFileUpload(file);

      tick(1500);

      expect(addCertSpy).not.toHaveBeenCalled();
      expect(component.errorMessage()).toContain('Only PDF, JPG, and PNG');
    }));

    it('should reject files over 5MB', fakeAsync(() => {
      const largeContent = new Array(6 * 1024 * 1024).fill('a').join('');
      const file = new File([largeContent], 'large.pdf', { type: 'application/pdf' });
      (component as any).handleFileUpload(file);

      tick(1500);

      expect(addCertSpy).not.toHaveBeenCalled();
      expect(component.errorMessage()).toContain('5MB');
    }));

    it('should enforce max certifications limit', fakeAsync(() => {
      component.certifications = Array(20).fill(null).map((_, i) => ({
        ...mockCertifications[0],
        id: `cert-${i}`
      }));
      component.maxCertifications = 20;
      fixture.detectChanges();

      const file = new File(['content'], 'new.pdf', { type: 'application/pdf' });
      (component as any).handleFileUpload(file);

      tick(1500);

      expect(addCertSpy).not.toHaveBeenCalled();
    }));

    it('should show upload progress', fakeAsync(() => {
      const file = new File(['content'], 'test.pdf', { type: 'application/pdf' });
      (component as any).handleFileUpload(file);

      expect(component.uploadingFile()).toBe('test.pdf');
      expect(component.uploadProgress()).toBe(0);

      tick(200);
      expect(component.uploadProgress()).toBe(20);

      tick(800);
      expect(component.uploadProgress()).toBe(100);
      expect(component.uploadingFile()).toBeNull();
    }));

    it('should handle dragover event', () => {
      const event = { preventDefault: jasmine.createSpy(), stopPropagation: jasmine.createSpy() } as any;
      component.onDragOver(event);

      expect(component.isDragOver()).toBe(true);
      expect(event.preventDefault).toHaveBeenCalled();
    });

    it('should handle dragleave event', () => {
      component.isDragOver.set(true);
      const event = { preventDefault: jasmine.createSpy(), stopPropagation: jasmine.createSpy() } as any;
      component.onDragLeave(event);

      expect(component.isDragOver()).toBe(false);
    });

    it('should handle file drop', fakeAsync(() => {
      const file = new File(['content'], 'dropped.pdf', { type: 'application/pdf' });
      const event = {
        preventDefault: jasmine.createSpy(),
        stopPropagation: jasmine.createSpy(),
        dataTransfer: { files: [file] }
      } as any;

      component.onFileDrop(event);

      tick(1500);

      expect(addCertSpy).toHaveBeenCalledWith(file);
    }));
  });

  describe('Removing Certifications', () => {
    let removeCertSpy: jasmine.Spy;

    beforeEach(() => {
      removeCertSpy = spyOn(component.removeCertification, 'emit');
      component.certifications = [...mockCertifications];
      fixture.detectChanges();
    });

    it('should emit removeCertification on confirm', () => {
      component.confirmRemoveCertification('cert-1');

      expect(removeCertSpy).toHaveBeenCalledWith('cert-1');
    });
  });

  describe('Utility Functions', () => {
    it('should format file size in bytes', () => {
      expect(component.formatFileSize(512)).toBe('512 B');
    });

    it('should format file size in KB', () => {
      expect(component.formatFileSize(2048)).toBe('2.0 KB');
    });

    it('should format file size in MB', () => {
      expect(component.formatFileSize(1536 * 1024)).toBe('1.5 MB');
    });
  });

  describe('Accessibility', () => {
    it('should have aria-labels on interactive elements', () => {
      component.currentSpecialties = [...mockSpecialties];
      fixture.detectChanges();

      const addBtn = fixture.nativeElement.querySelector('.btn--primary[aria-label]');
      expect(addBtn.getAttribute('aria-label')).toBeTruthy();

      const dragHandle = fixture.nativeElement.querySelector('.drag-handle[aria-label]');
      expect(dragHandle.getAttribute('aria-label')).toContain('Drag to reorder');

      const removeBtn = fixture.nativeElement.querySelector('.remove-btn[aria-label]');
      expect(removeBtn.getAttribute('aria-label')).toContain('Remove');
    });

    it('should have proper modal aria attributes', () => {
      component.openModal();
      fixture.detectChanges();

      const modal = fixture.nativeElement.querySelector('.modal');
      expect(modal.getAttribute('role')).toBe('dialog');
      expect(modal.getAttribute('aria-modal')).toBe('true');
      expect(modal.getAttribute('aria-labelledby')).toBe('modal-title');
    });

    it('should have proper role on upload area', () => {
      const uploadArea = fixture.nativeElement.querySelector('.upload-area');
      expect(uploadArea.getAttribute('role')).toBe('button');
      expect(uploadArea.getAttribute('tabindex')).toBe('0');
      expect(uploadArea.getAttribute('aria-label')).toContain('Upload certification');
    });
  });

  describe('Messages', () => {
    it('should show success message temporarily', fakeAsync(() => {
      (component as any).showSuccess('Test success');

      expect(component.successMessage()).toBe('Test success');

      tick(3000);

      expect(component.successMessage()).toBeNull();
    }));

    it('should show error message temporarily', fakeAsync(() => {
      (component as any).showError('Test error');

      expect(component.errorMessage()).toBe('Test error');

      tick(5000);

      expect(component.errorMessage()).toBeNull();
    }));
  });
});
