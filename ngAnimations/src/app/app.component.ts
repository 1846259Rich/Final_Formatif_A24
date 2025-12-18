import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, shakeX, tada } from 'ng-animate';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    standalone: true,
    animations: [
    trigger("SquareRed", [transition(":increment", useAnimation(shakeX, { params: { timing: 2 }}))]),
    trigger("SquareGreen", [transition(":increment", useAnimation(bounce, { params: { timing: 4 }}))]),
    trigger("SquareBlue", [transition(":increment", useAnimation(tada, { params: { timing: 3 }}))])
    ],
})
export class AppComponent {
  title = 'ngAnimations';

  shakeSR = 0;
  bounceSG = 0;
  tadaSB = 0;
  css_square = false;

  constructor() {
  }

  animateSquare(boucle:boolean = false){
    this.shakeSR++;
    setTimeout(() => { 
      this.bounceSG++; 

      setTimeout(() => { 
        this.tadaSB++; 
        if(boucle)
          setTimeout(() => { this.animateSquare(boucle) }, 3000);
      }, 4000);  
    }, 2000);
  }

  cssSquare(){
    this.css_square = true;
    setTimeout(() => { this.css_square = false }, 2000);
  }
}
