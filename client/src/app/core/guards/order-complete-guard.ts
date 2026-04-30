import { CanActivateFn } from '@angular/router';

export const orderCompleteGuard: CanActivateFn = (route, state) => {
  return true;
};
