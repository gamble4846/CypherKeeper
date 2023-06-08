import { LoginModel } from "./LoginModel";

export interface TokkenModel {
    LoginData: LoginModel;
    exp: Date;
    iss: string;
    aud: Array<string>;
    ServerData: string;
}