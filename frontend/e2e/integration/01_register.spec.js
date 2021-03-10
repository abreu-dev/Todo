/// <reference types="Cypress" />

describe('Register Page', function () {
    it('should visit register page', function () {
        cy.visit('/account/register');
        cy.wait(2000);

        cy.url().should('include', '/account/register');
    });
    it('should register', function () {
        cy.visit('/account/register');
        cy.wait(2000);

        cy.get('#username').type('tester');
        cy.wait(2000);

        cy.get('#email').type('tester@tester.com');
        cy.wait(2000);

        cy.get('#password').type('Tester%123');
        cy.wait(2000);

        cy.get('#register').click();
        cy.wait(10000);

        cy.url().should('include', '/home');
    });
});
