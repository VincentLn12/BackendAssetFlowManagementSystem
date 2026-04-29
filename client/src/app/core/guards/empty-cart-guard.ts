import { CanActivateFn } from '@angular/router';

export const emptyCartGuard: CanActivateFn = (route, state) => {
  return true;
};
