import { Component, HostListener, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { EditableMember } from '../../../types/member';
import { DatePipe } from '@angular/common';
import { MemberService } from '../../../core/services/member-service';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastService } from '../../../core/services/toast-service';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-member-profile',
  imports: [DatePipe, FormsModule],
  templateUrl: './member-profile.html',
  styleUrl: './member-profile.css'
})
export class MemberProfile implements OnInit, OnDestroy {
  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) notify($event: BeforeUnloadEvent) {
    if (this.editForm?.dirty) {
      $event.preventDefault();
    }
  }
  private accountService = inject(AccountService);  
  private toast = inject(ToastService);
  protected memberService = inject(MemberService);
  protected editableMember: EditableMember = {
    displayName: '',
    description: '',
    city: '',
    country: ''
  };

  constructor(){
  }
  
  ngOnInit(): void {
    this.editableMember = {
      displayName: this.memberService.member()?.displayName || '',
      description: this.memberService.member()?.description || '',
      city: this.memberService.member()?.city || '',
      country: this.memberService.member()?.country || ''
    };
  }

  updateProfile(){
    if(!this.memberService.member()) return;
    
    const updatedMember: EditableMember = {
      displayName: this.editableMember.displayName,
      description: this.editableMember.description,
      city: this.editableMember.city,
      country: this.editableMember.country
    };
    
    this.memberService.updateMember(updatedMember).subscribe({
      next: () => {
        const currentUser = this.accountService.currentUser();
        if(currentUser && updatedMember.displayName !== currentUser?.displayName)
        {
          currentUser.displayName = updatedMember.displayName;
          this.accountService.setCurrentUser(currentUser);
        }

        this.toast.success('Profile updated successfully');
        this.memberService.editMode.set(false);
        // Update the member signal with the new data
        const currentMember = this.memberService.member();
        if (currentMember) {
          this.memberService.member.set({
            ...currentMember,
            ...updatedMember
          });
        }
        this.editForm?.reset(this.editableMember);
      },
      error: (error) => {
        console.error('Update error:', error);
        this.toast.error('Failed to update profile');
      }
    });
  }
  ngOnDestroy(): void {
    if(this.memberService.editMode()) {
      this.memberService.editMode.set(false);
    }
  }
}
