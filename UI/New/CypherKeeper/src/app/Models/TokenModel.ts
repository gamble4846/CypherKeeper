import { LoginModel } from "./LoginModel";

export interface TokenModel {
    LoginData: LoginModel;
    exp: Date;
    iss: string;
    aud: Array<string>;
    ServerData: string;
}