import { Component, OnDestroy, OnInit } from "@angular/core";
import { User } from "oidc-client";
import { Subscription } from "rxjs";
import { AuthService } from "../services/auth.service";

@Component({
    template: ''
})
export abstract class AuthComponentBase implements OnInit {

    public userAuthenticated = false;

    constructor(protected authService: AuthService) {
        this.authService.loginChanged
            .subscribe(userAuthenticated => {
                this.userAuthenticated = userAuthenticated;
            })
    }
    ngOnInit(): void {
        this.authService.isAuthenticated()
            .then(userAuthenticated => {
                this.userAuthenticated = userAuthenticated;
            });
    }

    // public userHasRole(role: string): boolean {
    //     const roles: string[] = this.user == null ? '' : this.user.profile.role;
    //     return roles.findIndex(item => item === role) >= 0;
    // }

    // public get isAdmin(): boolean {
    //     //return this.userHasRole('admin');
    //     return true;
    // }
}
