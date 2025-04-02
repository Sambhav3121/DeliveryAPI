import UserModel from '../models/UserModel';
const API_BASE_URL = 'http://localhost:5040/api/User/profile';

export default class UserController {
    static async getUserProfile() {
        try {
            const response = await fetch(API_BASE_URL);
            const data = await response.json();
            return new UserModel(
                data.id,
                data.name,
                data.email,
                data.phone,
                data.gender,
                data.dob,
                data.address
            );
        } catch (error) {
            console.error('Error fetching user profile:', error);
        }
    }

    static async updateUserProfile(updatedUser) {
        try {
            const response = await fetch(API_BASE_URL, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(updatedUser),
            });
            const data = await response.json();
            console.log('Updated user:', data);
        } catch (error) {
            console.error('Error updating user profile:', error);
        }
    }
}
