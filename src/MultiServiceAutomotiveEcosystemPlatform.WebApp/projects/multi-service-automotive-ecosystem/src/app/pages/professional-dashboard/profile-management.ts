import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

interface Professional {
  id: string;
  businessName: string;
  personalName: string;
  title: string;
  businessType: string;
  bio: string;
  phone: string;
  email: string;
  website: string;
  address: string;
  city: string;
  state: string;
  zip: string;
}

@Component({
  selector: 'app-profile-management',
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './profile-management.html',
  styleUrl: './profile-management.scss',
})
export class ProfileManagement implements OnInit {
  profileForm: FormGroup;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private router: Router
  ) {
    this.profileForm = this.fb.group({
      businessName: ['', [Validators.required]],
      personalName: ['', [Validators.required]],
      title: [''],
      businessType: ['', [Validators.required]],
      bio: ['', [Validators.maxLength(500)]],
      phone: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      website: [''],
      address: [''],
      city: [''],
      state: [''],
      zip: ['']
    });
  }

  ngOnInit(): void {
    this.loadProfile();
  }

  private loadProfile(): void {
    // Mock data - replace with actual API call
    const mockProfile: Professional = {
      id: '1',
      businessName: 'Bob\'s Auto Service',
      personalName: 'Bob Smith',
      title: 'Owner',
      businessType: 'Auto Repair',
      bio: 'Serving the community for over 20 years with expert automotive services.',
      phone: '(555) 123-4567',
      email: 'bob@bobsauto.com',
      website: 'https://bobsauto.com',
      address: '123 Main St',
      city: 'Springfield',
      state: 'IL',
      zip: '62701'
    };

    this.profileForm.patchValue(mockProfile);
  }

  onSubmit(): void {
    if (this.profileForm.valid && !this.isSubmitting) {
      this.isSubmitting = true;
      
      const profileData: Professional = this.profileForm.value;
      
      // Mock API call - replace with actual API call
      console.log('Saving profile:', profileData);
      
      setTimeout(() => {
        this.isSubmitting = false;
        alert('Profile updated successfully!');
      }, 500);
    } else {
      Object.keys(this.profileForm.controls).forEach(key => {
        this.profileForm.controls[key].markAsTouched();
      });
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.profileForm.get(fieldName);
    return !!(field && field.invalid && field.touched);
  }

  getFieldError(fieldName: string): string {
    const field = this.profileForm.get(fieldName);
    if (field?.errors) {
      if (field.errors['required']) return 'This field is required';
      if (field.errors['email']) return 'Please enter a valid email address';
      if (field.errors['maxLength']) return `Maximum ${field.errors['maxLength'].requiredLength} characters`;
    }
    return '';
  }

  getBioCharacterCount(): string {
    const bioValue = this.profileForm.get('bio')?.value || '';
    return `${bioValue.length} / 500`;
  }
}
