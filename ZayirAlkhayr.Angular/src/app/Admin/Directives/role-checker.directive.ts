import { Directive, ElementRef, Input } from '@angular/core';
import { AuthService } from 'src/app/Auth/auth.service';

@Directive({
  selector: '[appRole]'
})
export class RoleCheckerDirective {
  @Input() roles: string[];

  constructor(private ref: ElementRef<HTMLElement>, private authService: AuthService) { }

  ngOnInit(): void {
    if (!this.authService.isInRole(this.roles))
      this.ref.nativeElement?.remove();
  }
}
