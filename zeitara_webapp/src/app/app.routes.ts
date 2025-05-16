import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ProductsPageComponent } from './components/products-page/products-page.component';
import { LikesComponent } from './components/likes/likes.component';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'products', component: ProductsPageComponent },
  { path: 'likes', component: LikesComponent }
];
