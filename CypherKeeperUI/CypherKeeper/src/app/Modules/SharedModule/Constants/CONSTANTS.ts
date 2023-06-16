import { MenuStyles } from "infinite-menu";

export const InterceptorSkipLoaderHeader = 'X-Skip-Loader-Interceptor';
export const InterceptorSkipTokkenHeader = 'X-Skip-Tokken-Interceptor';
export const InterceptorSkipErrorHeader = "X-Skip-Error-Interceptor";
export const InterceptorSkipDecryptHeader = "X-Skip-Decrypt-Interceptor";

export const PublicKeyForRSA = `-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC4ZpfUUHimSe1Z+9BVBIN1CG9i
LozZJt27tZMkmFeXqdxHcfKPBamD++pSVi9WpBL/hyuWpxNpPWFbRB6uexCQfQfp
2xyWPFkanczRiv4nuDsduPUve+RNhQtg8hG+mOt+3NIqW7gOK5/u57XQKv3wJiVM
KkzsaePTT5K+inuJvQIDAQAB
-----END PUBLIC KEY-----`;

export const AllowedDatabaseTypes = [
    "SQLServer"
]

export const MenuStylesConstant: Array<MenuStyles> = [
    { Location: 1, BackgroundColor: "#001528", TextColor: "white", PaddingLeft: "0px", OnHoverBackgroundColor: "black", OnHoverTextColor: "white" },
    { Location: 2, BackgroundColor: "#00284d", TextColor: "white", PaddingLeft: "10px", OnHoverBackgroundColor: "black", OnHoverTextColor: "white" },
    { Location: 3, BackgroundColor: "#004380", TextColor: "white", PaddingLeft: "20px", OnHoverBackgroundColor: "pink", OnHoverTextColor: "red" },
    { Location: 4, BackgroundColor: "#005eb3", TextColor: "white", PaddingLeft: "30px", OnHoverBackgroundColor: "black", OnHoverTextColor: "red" },
    { Location: 5, BackgroundColor: "#0078e6", TextColor: "white", PaddingLeft: "40px", OnHoverBackgroundColor: "black", OnHoverTextColor: "red" },
    { Location: 6, BackgroundColor: "#1a92ff", TextColor: "white", PaddingLeft: "50px", OnHoverBackgroundColor: "black", OnHoverTextColor: "red" },
    { Location: 7, BackgroundColor: "#4daaff", TextColor: "white", PaddingLeft: "60px", OnHoverBackgroundColor: "black", OnHoverTextColor: "red" },
    { Location: 8, BackgroundColor: "#80c2ff", TextColor: "grey", PaddingLeft: "70px", OnHoverBackgroundColor: "black", OnHoverTextColor: "red" },
    { Location: 9, BackgroundColor: "#b3dbff", TextColor: "grey", PaddingLeft: "80px", OnHoverBackgroundColor: "black", OnHoverTextColor: "red" },
];

// export const APIUrl = `http://offers-assess.at.ply.gg:60431/CypherKeeperAPI`;
export const APIUrl = `http://192.168.0.105/CypherKeeperAPI`;
// export const APIUrl = `https://localhost:44376`;
