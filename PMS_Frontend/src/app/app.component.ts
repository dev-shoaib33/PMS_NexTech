import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MenubarComponent } from "./component/menubar/menubar.component";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [RouterOutlet, MenubarComponent]
})
export class AppComponent {
  title = 'Product Management System';
}
