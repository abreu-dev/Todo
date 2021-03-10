/// <reference types="Cypress" />

describe('Login Page', function () {
    it('should visit login page', function () {
        cy.visit('/account/login');

        cy.url().should('include', '/account/login');
    });
    it('should login', function () {
        cy.visit('/');
        cy.wait(2000);

        cy.get('#username').type('tester');
        cy.wait(2000);

        cy.get('#password').type('Tester%123');
        cy.wait(2000);

        cy.get('#login').click();

        cy.wait(2000);
        cy.url().should('include', '/home');
    });
});
