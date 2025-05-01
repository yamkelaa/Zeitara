export interface UserLogin {
  username: string,
  password: string
}

export type Gender = 'male' | 'female' | 'other'

export interface UserRegistrationRequestDto {
  username: string;
  user_FirstName: string;
  user_LastName: string;
  password: string;
  age: number;
  gender: Gender;
  street: string;
  suburb: string;
  province: string;
  postal_Code: string;
  city: string;
  country: string;
}

export interface UserRegistrationResponse {
  success: boolean;
  message: string;
  user_Id?: number;
  username?: string;
}


