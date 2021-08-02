--
-- PostgreSQL database dump
--

-- Dumped from database version 13.2
-- Dumped by pg_dump version 13.2

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Data for Name: AspNetRoles; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."AspNetRoles" VALUES ('4c11fd92-3871-4684-ba6a-57ed1ebd77a7', 'Administrator', 'admin', 'ADMIN', 'f869fe85-8578-4ced-a419-60efd11b1aaf');

--
-- Data for Name: AspNetUsers; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."AspNetUsers" VALUES ('c6b9b260-9d81-457d-9897-6495f68bb34d', 'Super Admin', 'super', 'SUPER', 'super@abacuza.com', 'SUPER@ABACUZA.COM', false, 'AQAAAAEAACcQAAAAEOjRegiOfzVJqdZtZZfAB80OdNFWxXIBrLQ/8rv8Uhq2N6LMWwSLH/ZMKzn1Ioig4g==', 'M2QPQQWSS5NNMXOVE5CV3NWP7M4T7UYL', '19f80875-9c07-4b5b-808c-969fca9984c3', NULL, false, false, NULL, true, 0);
INSERT INTO public."AspNetUsers" VALUES ('2e5832e1-d112-4369-9742-b394f57930b9', 'Abacuza User', 'abacuza', 'ABACUZA', 'abacuza@abacuza.com', 'ABACUZA@ABACUZA.COM', false, 'AQAAAAEAACcQAAAAEHiCkTWGxyvIBDoshUPBF6aN/Ec59QlH4Z+gR6X6HkZFGIZpq/yFCd8WB90vnT/rcg==', 'XVHOYY62IS4Q5ESBCJLJZ34XN7NY3R4P', '88b3f45a-e57b-4200-bc4e-f4dde30a7cc3', NULL, false, false, NULL, true, 0);

--
-- Data for Name: AspNetUserClaims; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."AspNetUserClaims" VALUES (1, 'c6b9b260-9d81-457d-9897-6495f68bb34d', 'name', 'super');
INSERT INTO public."AspNetUserClaims" VALUES (2, 'c6b9b260-9d81-457d-9897-6495f68bb34d', 'nickname', 'Super Admin');
INSERT INTO public."AspNetUserClaims" VALUES (3, 'c6b9b260-9d81-457d-9897-6495f68bb34d', 'email', 'super@abacuza.com');
INSERT INTO public."AspNetUserClaims" VALUES (4, 'c6b9b260-9d81-457d-9897-6495f68bb34d', 'role', 'admin');
INSERT INTO public."AspNetUserClaims" VALUES (5, '2e5832e1-d112-4369-9742-b394f57930b9', 'name', 'abacuza');
INSERT INTO public."AspNetUserClaims" VALUES (6, '2e5832e1-d112-4369-9742-b394f57930b9', 'nickname', 'Abacuza User');
INSERT INTO public."AspNetUserClaims" VALUES (7, '2e5832e1-d112-4369-9742-b394f57930b9', 'email', 'abacuza@abacuza.com');

--
-- Data for Name: AspNetUserRoles; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."AspNetUserRoles" VALUES ('c6b9b260-9d81-457d-9897-6495f68bb34d', '4c11fd92-3871-4684-ba6a-57ed1ebd77a7');

--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."__EFMigrationsHistory" VALUES ('20210327114719_InitialCreate', '5.0.4');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20210327114815_InitialCreate', '5.0.4');


--
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."AspNetRoleClaims_Id_seq"', 1, false);


--
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."AspNetUserClaims_Id_seq"', 7, true);


--
-- PostgreSQL database dump complete
--
