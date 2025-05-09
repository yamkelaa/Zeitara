import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { UserLogin, UserLoginResponse, UserRegistrationRequestDto, UserRegistrationResponse } from "../shared/models/user/user";
import { Observable } from "rxjs";
import { ResponseDto } from "../shared/models/response/response";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly apiUrl: string;

  constructor(private readonly http: HttpClient) {
    this.apiUrl = environment.apiUrl;
  }

  loginUser(user: UserLogin): Observable<ResponseDto<UserLoginResponse>> {
    const loginUrl = `${this.apiUrl}/User/Login`;
    return this.http.post<ResponseDto<UserLoginResponse>>(loginUrl, user);
  }

  registerUser(registrationDetails: UserRegistrationRequestDto): Observable<ResponseDto<UserRegistrationResponse>> {
    const registrationUrl = `${this.apiUrl}/User/Register`
    return this.http.post<ResponseDto<UserRegistrationResponse>>(registrationUrl, registrationDetails)
  }


}
