import { Component, inject } from '@angular/core';
import { HeaderComponent } from './layout/header/header.component';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/product';
import { Pagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  imports: [HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'Skinet';
  baseUrl = environment.baseUrl;
  private http = inject(HttpClient);
  products: Product[] = [];

  ngOnInit() {
    this.http.get<Pagination<Product>>(this.baseUrl + 'products').subscribe({
      next: (response) => (this.products = response.data),
      error: (error) => console.error(error),
      complete: () => console.log('complete'),
    });
  }
}
