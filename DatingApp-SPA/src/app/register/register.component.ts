import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter<boolean>();
  model: any = {};


  constructor(private authService: AuthService) { }

  ngOnInit() {
    console.log(this.model)
  }

  cancel(){
    this.cancelRegister.emit(false)
    console.log('cancel')
  }

  register(){
    this.authService.register(this.model).subscribe(() => {
      console.log('registeration success')
    }, error => {
      console.log(error);

    })
  }

}
