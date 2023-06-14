export interface Server {
    gUIDServer: string;
    serverName: string;
    databaseType: string;
    connectionString: string;
    keyVerify: string;
}

export interface ServerViewModel {
    serverName: string;
    databaseType: string;
    connectionString: string;
    key: string;
}

export interface ServerDisplayModel {
    gUIDServer: string;
    serverName: string;
    databaseType: string;
}

export interface SelectedServerModel {
    gUIDServer: string;
    serverName: string;
    databaseType: string;
    connectionString: string;
    key: string;
}

export interface SelectServerModel {
    gUIDServer: string;
    key: string;
}