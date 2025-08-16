export type User = {
    id: string;
    displayName: string;
    email: string;
    token: string;
    imageUrl?: string;
}

export type LoginCredentials = {
    email: string;
    password: string;
}

export type RegisterCredentials = {
    displayName: string;
    email: string;
    password: string;
    gender: string;
    dateOfBirth: string;
    city: string;
    country: string;
}