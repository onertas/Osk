import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const roleGuard: CanActivateFn = (route, state) => {

  //var roles=inject(AuthService).getRoles()
  const router: Router = inject(Router);

  var sonuc:boolean=true
  var res=route.data['expectedroles'] as Array<string>


  // if(res!=undefined && res!=null){
  //    sonuc=roles.some((x) => res.includes(x))
  // }

  // if(sonuc==false){

  //   router.navigateByUrl("errorpage")

  // }

  return sonuc;
};
