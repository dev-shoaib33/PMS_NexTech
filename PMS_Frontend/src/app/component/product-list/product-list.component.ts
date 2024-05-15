import { CommonModule } from '@angular/common';
import { Component, NgModule, OnInit } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { Product } from '../../model/Product.model';
import { ProductsService } from '../../service/products.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { HttpClientModule, withFetch } from '@angular/common/http';
import { Console } from 'node:console';
import { GridResponseModel, ResponseModel } from '../../model/Response.model';
import { MatPaginatorModule } from '@angular/material/paginator';

interface ApiResponse {
  isSuccess: boolean;
  itemList?: Product[];
  errorMessage?: string;
}

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterLink, HttpClientModule, RouterModule, MatIconModule, MatTableModule, MatPaginatorModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductComponent implements OnInit {
  products: Product[] = [];
  errorMessage: string = '';
  constructor(private productService: ProductsService, private router: Router, private snackBar: MatSnackBar,) {

  }
  ngOnInit(): void {
    this.productService.getAllProducts(0, 10).subscribe(
      (response: GridResponseModel<Product>) => {
        if (response.isSuccess) {
          this.products = response.itemList || [];
        } else {
          if (response.isSuccess) {
            this.products = response.itemList || [];
          } else {
            this.errorMessage = response.message || 'Unknown error occurred';
            this.showSnackBar('Some Error Occured While fetching Products');
          }
        }
      },
      error => {
        this.showSnackBar('Some Error Occured While fetching Products');
        this.errorMessage = 'An error occurred while fetching products: ' + error.message;
      }
    );
  }
  deleteProduct(id: number) {
    this.productService.deleteProduct(id).subscribe({
      next: (response: ResponseModel<boolean>) => {
        if (response.isSuccess) {
          this.products = this.products.filter(x => x.productId != id);
          this.showSnackBar('Product deleted successfully');
        }
      }
    });
  }

  private showSnackBar(message: string) {
    this.snackBar.open(message, 'Close', { duration: 5000 });
  }

  onPageChange(event: any) {

    if (event.pageIndex < 0)
      return;

    this.productService.getAllProducts(event.pageIndex, event.pageSize).subscribe(
      (response: GridResponseModel<Product>) => {
        if (response.code == 200) {
          this.products = response.itemList || [];
        } else {
          if (response.isSuccess) {
            this.products = response.itemList || [];
          } else {
            this.errorMessage = response.message || 'Unknown error occurred';
            this.showSnackBar('Some Error Occured While fetching Products');
          }
        }
      },
      error => {
        this.showSnackBar('Some Error Occured While fetching Products');
        this.errorMessage = 'An error occurred while fetching products: ' + error.message;
      }
    );
    var a = event.pageIndex;
    var b = event.pageSize;
    // Make your API call here using your API service
    // // Pass any required parameters such as offset and pageSize to your API service method
    // this.apiService.fetchData(event.pageIndex, event.pageSize).subscribe(response => {
    //   // Handle the API response
    //   console.log('API Response:', response);
    // }, error => {
    //   // Handle API error
    //   console.error('API Error:', error);
    // });
  }

}


@NgModule({
  // declarations: [ProductComponent],
  // imports: [CommonModule,RouterModule,MatIconModule, MatTableModule]
})
export class ProductModule { }
