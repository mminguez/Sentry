﻿/*---------------------------------------------------------------------------------------------
 *  This file is an automatically generated Command Proxy
 *  
 *--------------------------------------------------------------------------------------------*/
import { Command } from  '@dolittle/commands';

export class GrantConsent extends Command
{
    constructor() {
        super();
        this.type = '10c24f5d-ca85-4a78-8a26-8b94fdd23c1a';

        this.tenant = '00000000-0000-0000-0000-000000000000';
        this.scopes = [];
        this.returnUrl = '';
        this.rememberConsent = false;
    }
}