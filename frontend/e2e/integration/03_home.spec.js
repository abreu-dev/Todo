/// <reference types="Cypress" />

Cypress.Commands.add('login', function () {
    cy.request({
        method: 'POST',
        url: 'https://localhost:5001/api/accounts/login',
        body: {
            username: 'tester',
            password: 'Tester%123',
        },
    }).then(function (response) {
        localStorage.setItem('todo.user', JSON.stringify(response.body));
    });
});

describe('Home Page', function () {
    beforeEach(function () {
        cy.login();
        cy.visit('/home');
    });
    it('should visit home page', function () {
        cy.url().should('include', '/home');
    });
    it('should display page title', function () {
        cy.get('#page-title').should('contain.text', 'Quadros');
    });
    it('should add a new board', function () {
        cy.get('#add-board').click();
        cy.wait(2000);

        cy.get('#board-title').type('Serise');
        cy.wait(2000);

        cy.get('#submit').click();
        cy.wait(2000);
    });
    it('should visit board management page', function () {
        cy.get('#board-0').click();
        cy.wait(2000);

        cy.url().should('include', '/board');
    });
});
