import { Navbar, Container, Nav, Badge, Button } from 'react-bootstrap';
import { Link, useNavigate } from 'react-router-dom';
import { useCart } from '../context/CartContext';
import { useState } from 'react';


const CustomNavbar = () => {
    const [userEmail, setUserEmail] = useState(() => {
        const email = localStorage.getItem('userEmail');
        const token = localStorage.getItem('token');

        //sadece ikisi de varsa kullanıcı giriş yapmış say
        if (token && email) {
            return email;
        }

        return null;    //yoksa kullanıcı giriş yapmamıştır.
    });

    const navigate = useNavigate();
    const { cartCount } = useCart();

    //Çıkış yapma fonksiyonu
    const handleLogout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('userEmail');
        setUserEmail(null); //state sıfırlama
        navigate('/login'); //login sayfasına yönlendirme
        window.location.reload();   //sayfayı yenileme navbar yüncellenmesi için
    };

    return (
        <Navbar bg="dark" variant="dark" expand="lg" className="shadow-sm sticky-top">
            <Container>
                <Navbar.Brand as={Link} to="/" className="fw-bold">
                    🚴‍♂️ BikeStore
                </Navbar.Brand>

                <Navbar.Toggle aria-controls="basic-navbar-nav" />

                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="ms-auto align-items-center">
                        <Nav.Link as={Link} to="/">Ana Sayfa</Nav.Link>
                        <Nav.Link as={Link} to="/">Bisikletler</Nav.Link>

                        {/* SEPET BUTONU */}
                        <Nav.Link as={Link} to="/cart" className="position-relative me-3">
                            🛒 Sepet
                            <Badge bg="danger" className="position-absolute top-0 start-100 translate-middle rounded-pill">
                                {cartCount}
                            </Badge>
                        </Nav.Link>

                        {/* DİNAMİK GİRİŞ/ÇIKIŞ ALANI */}
                        {userEmail ? (
                            // Kullanıcı Varsa:
                            <>
                                <span className="text-light me-3">Merhaba, {userEmail.split('@')[0]}</span>
                                <Button variant="outline-danger" size="sm" onClick={handleLogout}>
                                    Çıkış Yap
                                </Button>
                            </>
                        ) : (
                            // Kullanıcı Yoksa:
                            <>
                                <Nav.Link as={Link} to="/login" className="btn btn-outline-light ms-2 px-3 text-white">
                                    Giriş Yap
                                </Nav.Link>
                                <Nav.Link as={Link} to="/register" className="btn btn-warning ms-2 px-3 text-dark">
                                    Kayıt Ol
                                </Nav.Link>
                            </>
                        )}

                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default CustomNavbar;