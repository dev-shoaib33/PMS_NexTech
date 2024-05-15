import { Component, NgModule, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ProductsService } from '../../service/products.service';
import { Product } from '../../model/Product.model';
import { dropdown } from '../../model/dropdown.model';
import { ApiResponseModel, GridResponseModel } from '../../model/Response.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormBuilder, FormGroup, FormGroupDirective, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatOptionModule } from '@angular/material/core';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-create-edit-product',
  templateUrl: './create-edit-product.component.html',
  styleUrls: ['./create-edit-product.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule,
    RouterModule,
    FormsModule,
    MatFormFieldModule,
    MatOptionModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatSelectModule,
    CommonModule
  ]
})
export class CreateEditProductComponent implements OnInit {

  form: FormGroup;
  categories: dropdown[] = [];
  uomList: dropdown[] = [];
  editMode: boolean = false;
  productIdToUpdate: number | null = null;

  constructor(
    private productService: ProductsService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder
  ) {
    this.form = this.formBuilder.group({
      productName: ['', Validators.required],
      description: ['', Validators.required],
      productCode: ['', Validators.required],
      price: ['', [Validators.required, Validators.min(0)]],
      uom: ['', Validators.required],
      categoryLkpId: ['', Validators.required],
      productId: [0]
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.productIdToUpdate = +params['id'];
        this.editMode = true;
        this.fetchProductDetails();
      }
      this.fetchCategories();
      this.fetchUoms();
    });
  }

  private fetchProductDetails() {
    this.productService.getProductbyId(this.productIdToUpdate!).subscribe({
      next: (response: ApiResponseModel<Product>) => {
        if (response.isSuccess && response.data) {
          this.form.patchValue(response.data);
        } else {
          this.showSnackBar('Failed to fetch product details for editing.');
        }
      },
      error: (error) => {
        console.error('An error occurred while fetching product details for editing:', error);
        this.showSnackBar('An error occurred while fetching product details for editing.');
      }
    });
  }

  createOrUpdateProduct() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const productData: Product = this.form.value;

    const apiCall = this.editMode ? this.productService.updateProduct(productData) : this.productService.createProduct(productData);

    apiCall.subscribe(
      (response: ApiResponseModel<number>) => {
        if (response.isSuccess) {
          this.showSnackBar(this.editMode ? 'Product Updated Successfully' : 'Product Created Successfully.');
          this.router.navigate(['products']);
        } else {
          console.error(this.editMode ? 'Failed to update product.' : 'Failed to create product.');
          this.showSnackBar(this.editMode ? 'Failed to update product.' : 'Failed to create product.');
        }
      },
      (error: ApiResponseModel<number>) => {
        console.error(this.editMode ? 'An error occurred while updating product.' : 'An error occurred while creating product.');
        this.showSnackBar(this.editMode ? 'An error occurred while updating product.' : 'An error occurred while creating product.');
      }
    );
  }

  private fetchCategories() {
    this.productService.getCategories().subscribe(
      (response: GridResponseModel<dropdown>) => {
        if (response.isSuccess) {
          this.categories = response.itemList?.flat() || [];
        } else {
          this.showSnackBar('Failed to fetch product categories.');
        }
      },
      (error) => {
        console.error('An error occurred while fetching product categories:', error);
        this.showSnackBar('An error occurred while fetching product categories.');
      }
    );
  }

  private fetchUoms() {
    this.productService.getUoms().subscribe(
      (response: GridResponseModel<dropdown>) => {
        if (response.isSuccess) {
          this.uomList = response.itemList || [];
        } else {
          this.showSnackBar('Failed to fetch product categories.');
        }
      },
      (error: GridResponseModel<dropdown>) => {
        console.error('An error occurred while fetching product categories:', error);
        this.showSnackBar('An error occurred while fetching product categories.');
      }
    );
  }

  private showSnackBar(message: string) {
    this.snackBar.open(message, 'Close', { duration: 5000 });
  }

}
