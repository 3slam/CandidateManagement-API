import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { CandidateService } from '../Shared/Services/CandidateService';
import { Candidate } from '../Shared/Models/Candidate';
import { Skill } from '../Shared/Models/Skill';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SkillService } from '../Shared/Services/SkillService';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {

 
  private skillService =inject(SkillService);
  private candidateService = inject(CandidateService);
  candidates = signal<Candidate[]>([]);
  selectedCandidateId: number | null = null;
  candidateForm: Candidate = this.getEmptyCandidate();
  skillForm: Skill = this.getEmptySkill();
  selectedFile: File | null = null;
  uploadMessage = '';
  uploadError = '';
  showAddPopup = false;
  showEditPopup = false;
  showSkillPopup = false;
 

  ngOnInit(): void {
    this.loadCandidates();
  }

 loadCandidates(): void {
  this.candidateService.getCandidates().subscribe({
    next: (value) => {
      this.candidates.set(value);
    },
    error: (err) => {
      console.error('API ERROR:', err);
    }
  });
}
 
  openAddPopup() {
    this.candidateForm = this.getEmptyCandidate();
    this.showAddPopup = true;
  }

  closeAddPopup() {
    this.showAddPopup = false;
    this.candidateForm = this.getEmptyCandidate();
  }

  openEditPopup(candidate: Candidate) {
    this.candidateForm = { ...candidate };
    this.showEditPopup = true;
  }

  closeEditPopup() {
    this.showEditPopup = false;
    this.candidateForm = this.getEmptyCandidate();
  }

  openSkillPopup(candidateId: number) {
    this.selectedCandidateId = candidateId;
    this.skillForm = this.getEmptySkill();
    this.showSkillPopup = true;
  }

  closeSkillPopup() {
    this.showSkillPopup = false;
    this.skillForm = this.getEmptySkill();
    this.selectedCandidateId = null;
  }

  submitAddCandidate() {
    if (!this.validateCandidateForm()) {
      alert('Please fill all required fields');
      return;
    }

    this.candidateService.add(this.candidateForm).subscribe(
      (response: any) => {
        alert('Candidate added successfully');
        this.closeAddPopup();
        this.loadCandidates()
      },
      (error: any) => {
        alert('Error adding candidate: ' + (error.error?.message || error.message));
      }
    );
  }

  submitEditCandidate() {
    if (!this.validateCandidateForm()) {
      alert('Please fill all required fields');
      return;
    }

    this.candidateService.edit(this.candidateForm).subscribe(
      (response: any) => {
        alert('Candidate updated successfully');
        this.closeEditPopup();
        this.loadCandidates()
      },
      (error: any) => {
        alert('Error updating candidate: ' + (error.error?.message || error.message));
      }
    );
  }

  submitAddSkill() {
    if (!this.skillForm.name || !this.skillForm.gainDate) {
      alert('Please fill all skill fields');
      return;
    }

    if (this.hasNumbers(this.skillForm.name)) {
      alert('Skill name cannot contain numbers');
      return;
    }

    if (this.selectedCandidateId === null) {
      alert('No candidate selected');
      return;
    }

    this.skillService.addSkill(this.selectedCandidateId, this.skillForm).subscribe(
      (response: any) => {
        alert('Skill added successfully');
        this.closeSkillPopup();
        this.loadCandidates()
      },
      (error: any) => {
        alert('Error adding skill: ' + (error.error?.message || error.message));
      }
    );
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      if (!file.name.endsWith('.txt')) {
        this.uploadError = 'Please select a .txt file';
        this.selectedFile = null;
        return;
      }
      this.selectedFile = file;
      this.uploadError = '';
      this.uploadMessage = '';
    }
  }

  submitUploadFile() {
    if (!this.selectedFile) {
      this.uploadError = 'Please select a file';
      return;
    }

    this.candidateService.upload(this.selectedFile).subscribe(
      (response: any) => {
        this.uploadMessage = 'File uploaded successfully';
        this.uploadError = '';
        this.selectedFile = null;
        const fileInput = document.getElementById('fileInput') as HTMLInputElement;
        if (fileInput) {
          fileInput.value = '';
        }
        this.loadCandidates()
      },
      (error: any) => {
        this.uploadError = 'Error uploading file: ' + (error.error?.message || error.message);
        this.uploadMessage = '';
      }
    );
  }

  downloadExport() {
    this.candidateService.export().subscribe(
      (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = 'candidates.txt';
        document.body.appendChild(link);
        link.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(link);
      },
      (error: any) => {
        alert('Error downloading export: ' + (error.error?.message || error.message));
      }
    );
  }

  private validateCandidateForm(): boolean {
    return !!(
      this.candidateForm.name &&
      this.candidateForm.nickname &&
      this.candidateForm.email &&
      this.candidateForm.yearsOfExperience !== undefined
    );
  }

  private hasNumbers(str: string): boolean {
    return /\d/.test(str);
  }

  private getEmptyCandidate(): Candidate {
    return {
      name: '',
      nickname: '',
      email: '',
      yearsOfExperience: 0,
      maxNumSkills: undefined
    };
  }

  private getEmptySkill(): Skill {
    return {
      name: '',
      gainDate: ''
    };
  }
}
