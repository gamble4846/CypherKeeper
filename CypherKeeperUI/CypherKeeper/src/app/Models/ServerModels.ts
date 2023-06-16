export interface Server {
    guidServer: string;
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
    guidServer: string;
    serverName: string;
    databaseType: string;
    imageLink: string;
}

export interface SelectedServerModel {
    guidServer: string;
    serverName: string;
    databaseType: string;
    connectionString: string;
    key: string;
    imageLink: string;
}

export interface SelectServerModel {
    guidServer: string;
    key: string;
}