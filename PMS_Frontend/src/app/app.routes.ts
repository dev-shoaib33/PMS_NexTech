import { Component } from '@angular/core';
import { Routes } from '@angular/router';
import {ProductComponent} from "./component/product-list/product-list.component";
import { CreateEditProductComponent } from './component/create-edit-product/create-edit-product.component';
import { create } from 'domain';
export const routes: Routes = [
    {
      path: 'products',
      component: ProductComponent
    },
    {
      path: 'products/add',
      component: CreateEditProductComponent
    },
    {
      path: 'products/edit/:id',
      component: CreateEditProductComponent 
    },
    {
      path: '',
      redirectTo: '/products',
      pathMatch: 'full'
    },
    {
      path: '**',
      redirectTo: '/products'
    }
  ];
