/*
 * Public API Surface of multi-service-automotive-ecosystem-components
 */

export * from './lib/multi-service-automotive-ecosystem-components';

// Components
export * from './lib/customer-login/customer-login';
export * from './lib/customer-registration/customer-registration';

// Types
export type { AuthenticatedUser, AuthError } from './lib/customer-login/customer-login';
export type { RegisteredUser, RegistrationError, Referrer } from './lib/customer-registration/customer-registration';

