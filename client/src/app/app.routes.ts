import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule, ActivatedRoute } from '@angular/router';

import { LoginLayoutComponent } from './login-layout/login-layout.component';
import { MainLayoutComponent } from './main-layout/main-layout.component';
import { CategoriesComponent } from './categories/categories.component';
import { AddCategoryComponent } from './add-category/add-category.component';
import { EditCategoryComponent } from './edit-category/edit-category.component';
import { InventoryTransactionsComponent } from './inventory-transactions/inventory-transactions.component';
import { AddInventoryTransactionComponent } from './add-inventory-transaction/add-inventory-transaction.component';
import { EditInventoryTransactionComponent } from './edit-inventory-transaction/edit-inventory-transaction.component';
import { ProductsComponent } from './products/products.component';
import { AddProductComponent } from './add-product/add-product.component';
import { EditProductComponent } from './edit-product/edit-product.component';
import { SuppliersComponent } from './suppliers/suppliers.component';
import { AddSupplierComponent } from './add-supplier/add-supplier.component';
import { EditSupplierComponent } from './edit-supplier/edit-supplier.component';
import { ApplicationUsersComponent } from './application-users/application-users.component';
import { LoginComponent } from './login/login.component';
import { ApplicationRolesComponent } from './application-roles/application-roles.component';
import { AddApplicationRoleComponent } from './add-application-role/add-application-role.component';
import { RegisterApplicationUserComponent } from './register-application-user/register-application-user.component';
import { EditApplicationUserComponent } from './edit-application-user/edit-application-user.component';
import { AddApplicationUserComponent } from './add-application-user/add-application-user.component';
import { ProfileComponent } from './profile/profile.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';

import { SecurityService } from './security.service';
import { AuthGuard } from './auth.guard';
export const routes: Routes = [
  { path: '', redirectTo: '/categories', pathMatch: 'full' },
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      {
        path: 'categories',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: CategoriesComponent
      },
      {
        path: 'add-category',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: AddCategoryComponent
      },
      {
        path: 'edit-category/:CategoryID',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: EditCategoryComponent
      },
      {
        path: 'inventory-transactions',
        canActivate: [AuthGuard],
        data: {
          roles: ['Authenticated'],
        },
        component: InventoryTransactionsComponent
      },
      {
        path: 'add-inventory-transaction',
        canActivate: [AuthGuard],
        data: {
          roles: ['Authenticated'],
        },
        component: AddInventoryTransactionComponent
      },
      {
        path: 'edit-inventory-transaction/:TransactionID',
        canActivate: [AuthGuard],
        data: {
          roles: ['Authenticated'],
        },
        component: EditInventoryTransactionComponent
      },
      {
        path: 'products',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: ProductsComponent
      },
      {
        path: 'add-product',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: AddProductComponent
      },
      {
        path: 'edit-product/:ProductID',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: EditProductComponent
      },
      {
        path: 'suppliers',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: SuppliersComponent
      },
      {
        path: 'add-supplier',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: AddSupplierComponent
      },
      {
        path: 'edit-supplier/:SupplierID',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: EditSupplierComponent
      },
      {
        path: 'application-users',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: ApplicationUsersComponent
      },
      {
        path: 'application-roles',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: ApplicationRolesComponent
      },
      {
        path: 'add-application-role',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: AddApplicationRoleComponent
      },
      {
        path: 'register-application-user',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: RegisterApplicationUserComponent
      },
      {
        path: 'edit-application-user/:Id',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: EditApplicationUserComponent
      },
      {
        path: 'add-application-user',
        canActivate: [AuthGuard],
        data: {
          roles: ['SuperAdmin'],
        },
        component: AddApplicationUserComponent
      },
      {
        path: 'profile',
        canActivate: [AuthGuard],
        data: {
          roles: ['Authenticated'],
        },
        component: ProfileComponent
      },
      {
        path: 'unauthorized',
        data: {
          roles: ['Everybody'],
        },
        component: UnauthorizedComponent
      },
    ]
  },
  {
    path: '',
    component: LoginLayoutComponent,
    children: [
      {
        path: 'login',
        data: {
          roles: ['Everybody'],
        },
        component: LoginComponent
      },
    ]
  },
];

export const AppRoutes: ModuleWithProviders = RouterModule.forRoot(routes);
