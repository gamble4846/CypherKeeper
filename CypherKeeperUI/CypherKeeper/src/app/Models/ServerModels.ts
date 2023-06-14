export interface Server {
    gUIDServer: string;
    serverName: string;
    databaseType: string;
    connectionString: string;
    keyVerify: string;
    imageLink: string;
}

export interface ServerViewModel {
    serverName: string;
    databaseType: string;
    connectionString: string;
    key: string;
    imageLink: string;
}

export interface ServerDisplayModel {
    gUIDServer: string;
    serverName: string;
    databaseType: string;
    imageLink: string;
}

export interface SelectedServerModel {
    gUIDServer: string;
    serverName: string;
    databaseType: string;
    connectionString: string;
    key: string;
    imageLink: string;
}

export interface SelectServerModel {
    gUIDServer: string;
    key: string;
}