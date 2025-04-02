import { useEffect, useState } from 'react';
import UserController from '../controllers/UserController';

const UserProfile = () => {
    const [user, setUser] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchUserProfile = async () => {
            setIsLoading(true);
            try {
                const userData = await UserController.getUserProfile();
                setUser(userData);
            } catch (err) {
                setError('Error fetching user profile');
            } finally {
                setIsLoading(false);
            }
        };

        fetchUserProfile();
    }, []);

    if (isLoading) return <div>Loading...</div>;
    if (error) return <div>{error}</div>;

    return (
        <div>
            <h2>User Profile</h2>
            <div>
                <strong>Name:</strong> {user.name}
            </div>
            <div>
                <strong>Email:</strong> {user.email}
            </div>
            <div>
                <strong>Phone:</strong> {user.phone}
            </div>
            <div>
                <strong>Gender:</strong> {user.gender}
            </div>
            <div>
                <strong>Date of Birth:</strong> {user.dob}
            </div>
            <div>
                <strong>Address:</strong> {user.address}
            </div>
        </div>
    );
};

export default UserProfile;