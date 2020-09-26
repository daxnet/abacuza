import { Component } from '@angular/core';

@Component({
  selector: 'ngx-footer',
  styleUrls: ['./footer.component.scss'],
  template: `
    <span class="created-by">
      Copyright &copy; 2020 by Abacuza, all rights reserved.
    </span>
    <div class="socials">
      <a href="https://github.com/daxnet/abacuza" target="_blank" class="ion ion-social-github"></a>
    </div>
  `,
})
export class FooterComponent {
}
