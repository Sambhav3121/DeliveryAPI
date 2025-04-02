// src/views/UserProfileView.js
import React, { useState, useEffect } from 'react';
import UserController from '../controllers/UserController';

const UserProfileView = () => {
    const [user, setUser] = useState(null);

    useEffect(() => {
        const fetchedUser = UserController.getUserProfile();
        setUser(fetchedUser);
    }, []);

    const handleSubmit = (e) => {
        e.preventDefault();
        const updatedUser = {
            ...user,
            fullName: e.target.fullName.value,
            email: e.target.email.value,
            phoneNumber: e.target.phoneNumber.value,
            gender: e.target.gender.value,
            birthDate: e.target.birthDate.value,
            address: e.target.address.value,
        };
        UserController.updateUserProfile(updatedUser);
    };

    if (!user) return <div>Loading...</div>;

    return (
        <div>
            <h2>User Profile</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Full Name</label>
                    <input type="text" name="fullName" defaultValue={user.fullName} />
                </div>
                <div>
                    <label>Email</label>
                    <input type="email" name="email" defaultValue={user.email} />
                </div>
                <div>
                    <label>Phone Number</label>
                    <input type="text" name="phoneNumber" defaultValue={user.phoneNumber} />
                </div>
                <div>
                    <label>Gender</label>
                    <select name="gender" defaultValue={user.gender}>
                        <option value="Male">Male</option>
                        <option value="Female">Female</option>
                        <option value="Other">Other</option>
                    </select>
                </div>
                <div>
                    <label>Birth Date</label>
                    <input type="date" name="birthDate" defaultValue={user.birthDate} />
                </div>
                <div>
                    <label>Address</label>
                    <input type="text" name="address" defaultValue={user.address} />
                </div>
                <button type="submit">Update Profile</button>
            </form>
        </div>
    );
};

export default UserProfileView;
