import { Component, OnDestroy, OnInit } from "@angular/core";
import { User } from "oidc-client";
import { Subscription } from "rxjs";
import { AuthService } from "../services/auth.service";

@Component({
    template: ''
})
export abstract class AuthComponentBase implements OnInit, OnDestroy {

    public user: User | null = null;
    public isAdmin: boolean = false;
    private loginChangedSubscription: Subscription | null = null;

    constructor(protected authService: AuthService) {
        this.loginChangedSubscription = this.authService.loginChanged
            .subscribe(authenticated => {
                if (authenticated) {
                    this.user = this.authService.currentUser;
                    this.isAdmin = this.authService.isAdmin;
                } else {
                    this.user = null;
                }
            });
    }

    ngOnDestroy(): void {
        this.loginChangedSubscription?.unsubscribe();
    }

    async ngOnInit() {
        await this.authService.isAuthenticated();
        this.user = this.authService.currentUser;
        this.isAdmin = this.authService.isAdmin;
    }
}
