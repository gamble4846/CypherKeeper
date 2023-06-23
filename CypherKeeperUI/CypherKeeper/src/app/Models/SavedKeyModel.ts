import { tbTwoFactorAuthModel_ADD } from "./tbTwoFactorAuthModel";

export interface SavedKeyModel {
    key: ToSaveKey;
    stringKeyFields: ToSaveStringKeyField[];
    twoFactorAuths: tbTwoFactorAuthModel_ADD[];
}

export interface ToSaveKey {
    id: string | null;
    parentGroupId: string;
    name: string;
    userName: string;
    password: string;
    websiteId: string | null;
    notes: string;
}

export interface ToSaveStringKeyField {
    id: string | null;
    name: string;
    value: string;
}